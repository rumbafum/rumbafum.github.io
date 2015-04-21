'use strict';

angular.module('SACApp.athlete', [])
    .controller('AthleteCtrl', ['$scope', '$location', '$window', '$stateParams', 'AthleteService', 'RaceService',
        function ($scope, $location, $window, $stateParams, AthleteService, RaceService) {

            $scope.dateFormat = "dd/MM/yyyy";
            $scope.$root.title = 'Varitintas - Sintra a Correr';
            $scope.athlete = {};
            $scope.resultsData = [];
            
            $scope.$on('$viewContentLoaded', function () {
                $window.ga('send', 'pageview', { 'page': $location.path(), 'title': $scope.$root.title });
                $scope.init();
            });

            $scope.init = function () {
                AthleteService.getAthlete($stateParams.athleteId, $scope.bindData);
            }

            $scope.bindData = function (result) {
                $scope.athlete = result;
                RaceService.getRaceResultsByAthlete($stateParams.athleteId, function (response) {
                    response.forEach(function (r) {
                        $scope.resultsData.push(r);
                    });
                });
            }

        }]);