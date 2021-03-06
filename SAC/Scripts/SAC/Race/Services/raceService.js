﻿'use strict';

angular.module('SACApp.race').service('RaceService', ['$http', '$rootScope', function ($http, $rootScope) {
    var service = {};

    service.getRaces = function (callback) {
        return $http.get($rootScope.globals.serverUrl + 'api/Races/', {}).then(function (response) {
            callback(response.data);
        });
    };

    service.getRace = function (raceId, callback) {
        return $http.get($rootScope.globals.serverUrl + 'api/Races/' + raceId, {}).then(function (response) {
            callback(response.data);
        });
    };

    service.getRaceResultsByAgeRank = function (raceId, ageRankId, callback) {
        return $http.get($rootScope.globals.serverUrl + 'api/RaceResults/?raceId=' + raceId + '&ageRankId=' + ageRankId, {}).then(function (response) {
            callback(response.data);
        });
    }

    service.getRaceResultsByAthlete = function (athleteId, callback) {
        return $http.get($rootScope.globals.serverUrl + 'api/RaceResults/?athleteId=' + athleteId, {}).then(function (response) {
            callback(response.data);
        });
    }

    return service;

}]);