'use strict';

angular.module('SACApp.race', [])
    .controller('RaceCtrl', ['$scope', '$location', '$window', '$stateParams', 'RaceService', function ($scope, $location, $window, $stateParams, RaceService) {

        $scope.$root.title = 'Varitintas - Sintra a Correr';
        $scope.race = {};
        
        $scope.$on('$viewContentLoaded', function () {
            $window.ga('send', 'pageview', { 'page': $location.path(), 'title': $scope.$root.title });
            $scope.init();
        });

        $scope.init = function () {
            RaceService.getRace($stateParams.raceId, $scope.bindData);
        }

        $scope.bindData = function (response) {
            $scope.race = response;
        }

    }]);