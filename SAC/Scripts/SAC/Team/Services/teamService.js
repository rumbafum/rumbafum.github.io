'use strict';

angular.module('SACApp.team').service('TeamService', ['$http', '$rootScope', function ($http, $rootScope) {
    var service = {};

    service.getTeam = function (teamId, callback) {
        return $http.get($rootScope.globals.serverUrl + 'api/Teams/' + teamId, {}).then(function (response) {
            callback(response.data);
        });
    };

    service.getClassification = function (ageRankId, callback) {
        return $http.get($rootScope.globals.serverUrl + 'api/Teams/?ageRankId=' + ageRankId, {}).then(function (response) {
            callback(response.data);
        });
    };

    service.getClassificationByRace = function (raceId, callback) {
        return $http.get($rootScope.globals.serverUrl + 'api/Teams/?raceId=' + raceId, {}).then(function (response) {
            callback(response.data);
        });
    };

    service.getClassificationByRaceAndAgeRank = function (raceId, ageRankId, callback) {
        return $http.get($rootScope.globals.serverUrl + 'api/Teams/?raceId=' + raceId + '&ageRankId=' + ageRankId, {}).then(function (response) {
            callback(response.data);
        });
    };

    return service;

}]);