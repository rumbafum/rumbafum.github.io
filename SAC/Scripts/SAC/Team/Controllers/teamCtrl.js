'use strict';

angular.module('SACApp.team', [])
    .controller('TeamCtrl', ['$scope', '$location', '$window', '$stateParams', 'AthleteService', 'TeamService',
        function ($scope, $location, $window, $stateParams, AthleteService, TeamService) {

            $scope.$root.title = 'Varitintas - Sintra a Correr';
            $scope.team = {};
            $scope.athletesData = [];

            $scope.$on('$viewContentLoaded', function () {
                $window.ga('send', 'pageview', { 'page': $location.path(), 'title': $scope.$root.title });
                $scope.init();
            });

            $scope.init = function () {
                TeamService.getTeam($stateParams.teamId, $scope.bindData);
            }

            $scope.bindData = function (result) {
                $scope.team = result;
                AthleteService.getAthletesByTeam($stateParams.teamId, function (response) {
                    response.forEach(function (r) {
                        $scope.athletesData.push(r);
                    });
                });
            }

        }]);