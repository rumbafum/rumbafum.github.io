'use strict';

angular.module('SACApp.home', [])
    .controller('HomeCtrl', ['$scope', '$location', '$window', 'RaceService', function ($scope, $location, $window, RaceService) {
        
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

        $scope.dateFormat = "dd/MM/yyyy";
        $scope.$root.title = 'Varitintas - Sintra a Correr';
        $scope.listData = [];
        $scope.mode = Mode.single;
        $scope.selection = [];

        $scope.$on('$viewContentLoaded', function () {
            $window.ga('send', 'pageview', { 'page': $location.path(), 'title': $scope.$root.title });
            $scope.init();
        });

        $scope.init = function(){
            RaceService.getRaces($scope.bindData);
        }

        $scope.bindData = function (response) {
            response.forEach(function (item) {
                $scope.listData.push(item);
            });
        }

        $scope.itemClicked = function (event) {
            $location.path('/race/' + $scope.listData[$scope.selection[0]].Id);
        }

    }]);