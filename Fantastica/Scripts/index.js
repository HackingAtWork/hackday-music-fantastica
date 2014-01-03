var getCleanTime = function(time)
{
    return time == "00" ? "" : time + ":";
};

String.prototype.toHHMMSS = function () {
    var sec_num = parseInt(this, 10); // don't forget the second parm
    var hours = Math.floor(sec_num / 3600);
    var minutes = Math.floor((sec_num - (hours * 3600)) / 60);
    var seconds = sec_num - (hours * 3600) - (minutes * 60);

    if (hours < 10) { hours = "0" + hours; }
    if (minutes < 10) { minutes = "0" + minutes; }
    if (seconds < 10) { seconds = "0" + seconds; }
    var time = getCleanTime(hours) + minutes + ':' + seconds;
    return time;
};

var algoDj = angular.module('algoDj', ['ngAutocompleteModule']);

algoDj.factory('eventBus', function () {
    var cache = {};
    return {
        publish: function (topic, args) {
            cache[topic] && angular.forEach(cache[topic], function (callback) {
                callback.apply(null, args || []);
            });
        },

        subscribe: function (topic, callback) {
            if (!cache[topic]) {
                cache[topic] = [];
            }
            cache[topic].push(callback);
            return [topic, callback];
        },

        unsubscribe: function (handle) {
            var t = handle[0];
            cache[t] && angular.forEach(cache[t], function (idx) {
                if (this == handle[1]) {
                    cache[t].splice(idx, 1);
                }
            });
        }
    };
});

algoDj.factory('mp3service', function ($http) {

    var buildRequest = function (filterBy) {
        return 'artist=' + filterBy.artist +
            '&title=' + filterBy.title +
            '&album=' + filterBy.album;
    };
    return {

        getFileList: function (filterObj, max_results) {
            var max = max_results || 500;
            var requestArgs = buildRequest(filterObj);
            return $http.get('/fantastica/api/library?' + requestArgs).then(
				function (result) {
				    return result.data;
				}
			);
        },
        getNextSong: function () {
            return $http.get('/fantastica/home/NextSong').then(
                function (result) {
                    return result.data; //'/fantastica/content/mp3s/' + result.data;
                });
        }
    };
});

algoDj.factory('userService', function ($http) {
    return {
        getDjs: function (roomName) {
            return [
                { name: "Neonidas Basspalooza", avatar: "/Fantastica/content/images/suit.png" },
                { name: "Greg", avatar: "/Fantastica/content/images/suit.png" },
                { name: "Keith", avatar: "/Fantastica/content/images/suit.png" }
            ];
        }
    };
});


algoDj.directive('audioPlayer', ['$timeout', 'eventBus', function ($timeout, bus) {
    return {
        restrict: 'E',
        replace: true,
        template: '    <audio>' +
    	          '        <source src=" {{ song.data }}" /> Your browser does not support the audio element. ' +
        	      '    </audio>',
        link: function ($scope, element, attrs) {
            var audio = element[0];
            var playing = true;

            bus.subscribe('nextSongLoaded', function (song) {

                $scope.song = song;

                $timeout(function () {
                    audio.load();
                    audio.play();
                    $scope.playing = true;
                }, 250);
            });

            bus.subscribe('playToggled', function () {
                if (playing) {
                    audio.pause();
                } else {
                    audio.play();
                }

                playing = !playing;
            });


            element.bind('ended', function () {
                bus.publish('songEnded');
            });

            element.bind('timeupdate', function (event) {
                var time = Math.floor(event.target.currentTime);
                var total_time = Math.floor(event.target.duration);
                var percent_complete = event.target.currentTime / total_time * 100;

                bus.publish('tick', [{ time: time.toString().toHHMMSS(), total_time: total_time.toString().toHHMMSS(), percent_complete: percent_complete }]);
            });

            bus.publish('songEnded');
        }
    };
}]);

algoDj.directive('ngAudio', ['$timeout', 'eventBus', function ($timeout, bus) {
    // Runs during compile
    return {
        restrict: 'E', // E = Element, A = Attribute, C = Class, M = Comment
        replace: true,
        templateUrl: 'ngAudioTmpl.html',
        link: function (scope, element, attrs) {
            scope.playing = false;
            scope.togglePlaying = function () {
                bus.publish('playToggled');
                scope.playing = !scope.playing;
            };
            scope.nextClicked = function () {
                bus.publish('songEnded');
            };

            bus.subscribe('tick', function (data) {
                scope.time = data.time;
                scope.total_time = data.total_time;
                scope.percent_complete = data.percent_complete;
                scope.safeApply();
            });

            bus.subscribe('nextSongLoaded', function (song) {
                scope.currently_playing = song;
            });
        }
    };
}]);

algoDj.directive('ngDj', ['$timeout', function ($timeout) {
    return {
        restrict: 'E',
        replace: true,
        template: '<li>' +
                  '  <img height="50px" width="50px" src="{{dj.avatar}}" />' +
                  '  <div>{{dj.name}}</div>' +
                  '</li>'
    };
}]);

algoDj.controller('homeCtrl', function ($scope, $timeout, mp3service, userService, eventBus) {

    $scope.playlist = [];
    $scope.playing = false;
    $scope.readyToPlay = false;
    $scope.currently_playing = {};

    $scope.getFiles = function (input, max_results) {
        return mp3service.getFileList({ title: input, artist: '', album: '' }, max_results);
    };

    $scope.clicked = function (selected) {

        var itemIndex = $scope.playlist.indexOf(selected);
        if (itemIndex == -1) {
            $scope.playlist.unshift(selected);
        } else {
            var moved = $scope.playlist.splice(itemIndex, 1)[0];
            $scope.playlist.unshift(moved);
        }

        if ($scope.readyToPlay != true) {
            $scope.readyToPlay = true;
        }
    };

    eventBus.subscribe('songEnded', function () {
        $scope.currently_playing = mp3service.getNextSong()
            .then(function (data) {
                eventBus.publish('nextSongLoaded', [{artist:'bees', title:'sting sting', data: '/fantastica/content/mp3s/' + data }]);
            });
    });

    $timeout(function () {
        $scope.djs = userService.getDjs("");
    }, 500);


    $scope.safeApply = function (fn) {
        var phase = this.$root.$$phase;
        if (phase == '$apply' || phase == '$digest') {
            if (fn && (typeof (fn) === 'function')) {
                fn();
            }
        } else {
            this.$apply(fn);
        }
    };

});