'use strict';

angular.module('SACApp.athlete').service('AthleteService', ['$http', '$rootScope', function ($http, $rootScope) {
    var service = {};

    service.getAthlete = function (athleteId, callback) {
        return $http.get($rootScope.globals.serverUrl + 'api/Athletes/' + athleteId, {}).then(function (response) {
            callback(response.data);
        });
    };

    service.getAthletesByTeam = function (teamId, callback) {
        return $http.get($rootScope.globals.serverUrl + 'api/Athletes/?teamId=' + teamId, {}).then(function (response) {
            callback(response.data);
        });
    };

    service.getClassification = function (ageRankId, callback) {
        return $http.get($rootScope.globals.serverUrl + 'api/Athletes/?ageRankId=' + ageRankId, {}).then(function (response) {
            callback(response.data);
        });
    };

    return service;

}]);