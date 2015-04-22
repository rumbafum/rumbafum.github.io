'use strict';

angular.module('SACApp.athlete', [])
    .controller('AthleteModule', ['$scope', '$location', '$window',
        function ($scope, $location, $window) {

            $scope.$root.title = 'Varitintas - Sintra a Correr';

        }]);