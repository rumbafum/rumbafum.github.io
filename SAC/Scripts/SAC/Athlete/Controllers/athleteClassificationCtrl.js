'use strict';

angular.module('SACApp.athlete')
    .controller('AthleteClassificationCtrl', ['$scope', '$location', '$window', '$stateParams', 'AthleteService', 'AgeRankService',
        function ($scope, $location, $window, $stateParams, AthleteService, AgeRankService) {

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
            $scope.resultsData = [];
            $scope.ageRankData = [];
            $scope.ageRankSelection = [];
            $scope.mode = Mode.single;

            $scope.$on('$viewContentLoaded', function () {
                $window.ga('send', 'pageview', { 'page': $location.path(), 'title': $scope.$root.title });
                $scope.init();
            });

            $scope.init = function () {
                AgeRankService.getAgeRanks(function (response) {
                    response.forEach(function (ageRank) {
                        $scope.ageRankData.push(ageRank);
                    });
                    $scope.ageRankSelection = [0];
                    AthleteService.getClassification($scope.ageRankData[0].Id, $scope.bindData);
                });
            }

            $scope.bindData = function (result) {
                $scope.resultsData = [];
                result.forEach(function (r) {
                    $scope.resultsData.push(r);
                });
            }

            $scope.ageRankClicked = function (event) {
                AthleteService.getClassification($scope.ageRankData[$scope.ageRankSelection[0]].Id, $scope.bindData);
            }

        }]);