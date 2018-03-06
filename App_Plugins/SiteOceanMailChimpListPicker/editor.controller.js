angular.module("umbraco")
    .controller("SiteOcean.MailChimp.ListPickerController", function ($scope, $http, notificationsService) {

        $scope.model.lists = [];
        $scope.model.status = 'loading';

        $http.get('/umbraco/backoffice/api/mailchimpnewsletter/lists').then(function (response) {
            $scope.model.lists = response.data.lists;
            $scope.model.status = 'ready';
        }, function(error) {
            $scope.model.status = 'error';
            notificationsService.error("Couldn't load MailChimps newsletters list");
        });
});