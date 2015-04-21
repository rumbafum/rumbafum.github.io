'use strict';

angular.module('SACApp.team', [])
    .controller('TeamCtrl', ['$scope', '$location', '$window', '$stateParams', 'AthleteService', 'RaceService',
        function ($scope, $location, $window, $stateParams, AthleteService, RaceService) {

            $scope.$root.title = 'Varitintas - Sintra a Correr';
            $scope.team = {};
            $scope.athletesData = [];

            $scope.$on('$viewContentLoaded', function () {
                $window.ga('send', 'pageview', { 'page': $location.path(), 'title': $scope.$root.title });
                $scope.init();
            });

            $scope.init = function () {
                //AthleteService.getAthlete($stateParams.athleteId, $scope.bindData);
            }

            $scope.bindData = function (result) {
                //$scope.athlete = result;
                //RaceService.getRaceResultsByAthlete($stateParams.athleteId, function (response) {
                //    response.forEach(function (r) {
                //        $scope.resultsData.push(r);
                //    });
                //});
            }

        }]);