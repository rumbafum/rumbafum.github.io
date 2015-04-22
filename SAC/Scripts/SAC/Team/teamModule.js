'use strict';

angular.module('SACApp.team', [])
    .controller('TeamModule', ['$scope', '$location', '$window',
        function ($scope, $location, $window) {

            $scope.$root.title = 'Varitintas - Sintra a Correr';
            
        }]);