'use strict';

angular.module('SACApp.ageRank', []).service('AgeRankService', ['$http', '$rootScope', function ($http, $rootScope) {
    var service = {};

    service.getAgeRanks = function (callback) {
        return $http.get($rootScope.globals.serverUrl + 'api/AgeRanks/', {}).then(function (response) {
            callback(response.data);
        });
    };

    return service;

}]);