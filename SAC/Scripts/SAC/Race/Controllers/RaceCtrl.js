'use strict';

angular.module('SACApp.race', [])
    .controller('RaceCtrl', ['$scope', '$location', '$window', '$stateParams', 'RaceService', 'AgeRankService', 'TeamService',
        function ($scope, $location, $window, $stateParams, RaceService, AgeRankService, TeamService) {

            var Mode = {
                single: {
                    text: 'single',
                    selectionMode: WinJS.UI.SelectionMode.single,
                    tapBehavior: WinJS.UI.TapBehavior.directSelect
                },
                readOnly: {
                    text: "readonly",
                    selectionMode: "none",
                    tapBehavior: "none"
                }
            };

        $scope.$root.title = 'Varitintas - Sintra a Correr';
        $scope.race = {};
        $scope.ageRankData = [];
        $scope.resultsData = [];
        $scope.teamResultsData = [];
        $scope.allTeamResultsData = [];
        $scope.ageRankSelection = [];
        $scope.mode = Mode.single;
        $scope.gridMode = Mode.readOnly;
        
        $scope.$on('$viewContentLoaded', function () {
            $window.ga('send', 'pageview', { 'page': $location.path(), 'title': $scope.$root.title });
            $scope.init();
        });

        $scope.init = function () {
            RaceService.getRace($stateParams.raceId, $scope.bindData);
        }

        $scope.bindData = function (response) {
            TeamService.getClassificationByRace($stateParams.raceId, function (result) {
                result.forEach(function (r) {
                    $scope.allTeamResultsData.push(r);
                });
            });
            $scope.race = response;
            AgeRankService.getAgeRanks(function (response) {
                response.forEach(function (ageRank) {
                    $scope.ageRankData.push(ageRank);
                });
                $scope.ageRankSelection = [0];
                $scope.getResults();
                $scope.getTeamResults();
            });
        }

        $scope.getResults = function () {
            RaceService.getRaceResultsByAgeRank($scope.race.Id, $scope.ageRankData[$scope.ageRankSelection[0]].Id, function (result) {
                $scope.resultsData = [];
                result.forEach(function (r) {
                    $scope.resultsData.push(r);
                });
            });
        }

        $scope.getTeamResults = function () {
            TeamService.getClassificationByRaceAndAgeRank($stateParams.raceId, $scope.ageRankData[$scope.ageRankSelection[0]].Id, function (result) {
                $scope.teamResultsData = [];
                result.forEach(function (r) {
                    $scope.teamResultsData.push(r);
                });
            });
        }

        $scope.ageRankClicked = function (event) {
            $scope.getResults();
            $scope.getTeamResults();
        }

    }]);