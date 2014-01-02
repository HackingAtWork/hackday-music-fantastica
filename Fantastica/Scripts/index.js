var mp3Reader = angular.module('mp3Reader', ['ngAutocompleteModule']);

mp3Reader.factory('mp3service', function($http){
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

mp3Reader.directive('audioPlayer', ['$timeout', function($timeout){
	return {
		restrict: 'E',
		replace: true,
		template: '    <audio controls>' +
    	          '        <source src="{{ currently_playing.path }}" /> Your browser does not support the audio element. ' +
        	      '    </audio>',
    	link: function($scope, element, attrs){
    		audio = element[0];

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
                    playing = true;
                }, 250);
            };

    		element.bind('ended', setSong);

			setSong();
		}
    };
}]);

mp3Reader.directive('ngAudio', ['$timeout', function($timeout){
	// Runs during compile
	return {
		restrict: 'E', // E = Element, A = Attribute, C = Class, M = Comment
        replace: true,
        template: '<div>' +
        		  '    <audio-player></audio-player>' +
                  '    <div> Now playing: {{currently_playing.title }} by {{ currently_playing.artist }}</div>' +
                  '    <div> Upcoming:' +
                  '       <div ng-repeat="item in playlist" ng-if="item.$$hashKey != currently_playing.$$hashKey">{{ item.title }} by {{ item.artist }}</div>' +
                  '    </div>' + 
                  '</div>',
		link: function($scope, element, attrs) {
		}
	};
}]);

mp3Reader.controller('homeCtrl', function($scope, $timeout, mp3service){

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

	$scope.getNextSong = function(){
		var to_play = $scope.playlist.splice(0,1)[0];
		
		if(to_play !== undefined){
			$scope.playlist.push(to_play);
			return to_play;
		}
	}
});