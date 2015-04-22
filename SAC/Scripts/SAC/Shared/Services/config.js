'use strict';

angular.module('SACApp').factory('errorInterceptor', ['$q', '$injector', '$rootScope', '$location',
    function ($q, $injector, $rootScope, $location) {
        var notificationChannel;
        var $http;
        return {
            request: function (request) {
                notificationChannel = notificationChannel || $injector.get('requestNotificationChannel');
                // send a notification requests are complete
                notificationChannel.requestStarted();
                return request;
            },
            requestError: function (request) {
                return $q.reject(request);
            },
            response: function (response) {
                $http = $http || $injector.get('$http');
                if ($http.pendingRequests.length < 1) {
                    notificationChannel = notificationChannel || $injector.get('requestNotificationChannel');
                    notificationChannel.requestEnded();
                }
                return response;
            },
            responseError: function (response) {
                $http = $http || $injector.get('$http');
                if ($http.pendingRequests.length < 1) {
                    notificationChannel = notificationChannel || $injector.get('requestNotificationChannel');
                    notificationChannel.requestEnded();
                }
                if (response && response.status === 404) {
                }
                if (response && response.status >= 500) {
                }
                if (response && response.status === 401) {
                    //var action = new SkillsApp.Action(SkillsApp.ActionType.SessionExpired, { message: 'Your session as expired. Please login again.' });
                    //ActionNotificationService.dispatchNotification(action);
                    //$location.path('/login');
                }
                return $q.reject(response);
            }
        };
    }]);
