var algoDj = angular.module('algoDj', ['ngAutocompleteModule']);

algoDj.factory('mp3service', function($http){
	return {
		getFileList: function(filterString, max_results){
		    var max = max_results || 500;
		    return $http.get('/fantastica/api2/playlist').then(
				function(result){
					return result.data;
				}
			);
		}
	};
});

algoDj.factory('userService', function ($http) {
    return {
        getDjs: function (roomName) {
            return [
                { name: "Neonidas Basspalooza", avatar: "Fantastica/content/images/suit.png" },
                { name: "Greg", avatar: "Fantastica/content/images/suit.png" },
                { name: "Keith", avatar: "Fantastica/content/images/suit.png" }
            ];
        }
    };
});


algoDj.directive('audioPlayer', ['$timeout', function($timeout){
	return {
		restrict: 'E',
		replace: true,
		template: '    <audio>' +
    	          '        <source src="Fantastica/content/mp3s/fables_01_01_aesop.mp3" /> Your browser does not support the audio element. ' +
        	      '    </audio>',
    	link: function($scope, element, attrs){
    		audio = element[0];
    		$scope.time = 0;
    		$scope.percent_complete = 0;

            $scope.$watch('readyToPlay', function(newVal, oldVal){
                if(newVal !== undefined && newVal === true){
                	if(!$scope.playing){
                    	setSong();
                	}
                }
            }, true);

            var setSong = function(){
                $scope.currently_playing = $scope.getNextSong();

                $timeout(function(){
                    audio.load();
                    audio.play();
                    $scope.playing = true;
                }, 250);
            };

            $scope.togglePlaying = function () {
                if($scope.playing) {
                    audio.pause();
                } else {
                    audio.play();
                }

                $scope.playing = !$scope.playing;
            };

    		element.bind('ended', setSong);
    		element.bind('timeupdate', function (event) {
    		    $scope.time = Math.floor(event.target.currentTime);
    		    $scope.percent_complete = event.target.currentTime / event.target.duration * 100;
    		    if($scope.$$phase !== '$apply'){ $scope.$apply(); }
    		});

			setSong();
		}
    };
}]);

algoDj.directive('ngAudio', ['$timeout', function($timeout){
	// Runs during compile
	return {
		restrict: 'E', // E = Element, A = Attribute, C = Class, M = Comment
        replace: true,
        templateUrl: 'ngAudioTmpl.html',
        link: function (scope, element, attrs) {
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

algoDj.controller('homeCtrl', function($scope, $timeout, mp3service, userService){

    $scope.playlist = [];
    $scope.playing = false;
    $scope.readyToPlay = false;

    $scope.getFiles = function(input, max_results){
    	return mp3service.getFileList(input, max_results);
    }

	$scope.clicked = function(selected){

		var itemIndex = $scope.playlist.indexOf(selected);
		if(itemIndex == -1){
			$scope.playlist.unshift(selected);
		} else{
			var moved = $scope.playlist.splice(itemIndex,1)[0];
			$scope.playlist.unshift(moved);
		}

		if ($scope.readyToPlay != true){
			$scope.readyToPlay = true;
		}
	};

	$scope.getNextSong = function () {
	    var to_play = $scope.playlist.splice(0, 1)[0];

	    if (to_play !== undefined) {
	        $scope.playlist.push(to_play);
	        return to_play;
	    }
	};

	$timeout(function () {
	    $scope.djs = userService.getDjs("");
	}, 500);

});