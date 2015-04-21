'use strict';

// Declares how the application should be bootstrapped. See: http://docs.angularjs.org/guide/module
var app = angular.module('SACApp', ['winjs', 'ngSanitize', 'ui.router', 'SACApp.race', 'SACApp.ageRank', 'SACApp.home']);


app.config(['$stateProvider', '$locationProvider', '$httpProvider', function ($stateProvider, $locationProvider, $httpProvider) {

    $stateProvider
        .state('home', {
            url: '/',
            templateUrl: '/views/index',
            controller: 'HomeCtrl'

        })
        .state('race', {
            url: '/race/:raceId',
            templateUrl: '/views/race',
            controller: 'RaceCtrl'
        })
        //.state('about', {
        //    url: '/about',
        //    templateUrl: '/views/about',
        //    controller: 'AboutCtrl'
        //})
        //.state('login', {
        //    url: '/login',
        //    layout: 'basic',
        //    templateUrl: '/views/login',
        //    controller: 'LoginCtrl'
        //})
        .state('otherwise', {
            url: '*path',
            templateUrl: '/views/404',
            controller: 'Error404Ctrl'
        });

    $locationProvider.html5Mode({
        enabled: true,
        requireBase: false
    });

}]);

// Gets executed after the injector is created and are used to kickstart the application. Only instances and constants
// can be injected here. This is to prevent further system configuration during application run time.
app.run(['$templateCache', '$rootScope', '$state', '$stateParams', function ($templateCache, $rootScope, $state, $stateParams) {

    // <ui-view> contains a pre-rendered template for the current view
    // caching it will prevent a round-trip to a server at the first page load
    var view = angular.element('#ui-view');
    $templateCache.put(view.data('tmpl-url'), view.html());

    // Allows to retrieve UI Router state information from inside templates
    $rootScope.$state = $state;
    $rootScope.$stateParams = $stateParams;

    $rootScope.globals = {};
    $rootScope.globals.serverUrl = '/';

    $rootScope.$on('$stateChangeSuccess', function (event, toState) {

        // Sets the layout name, which can be used to display different layouts (header, footer etc.)
        // based on which page the user is located
        $rootScope.layout = toState.layout;
    });
}]);