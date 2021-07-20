node {
    stage('Clean workspace') {
        cleanWs()           
    }
    stage('Git Checkout') {
        git branch: 'develop_ci-cd', credentialsId: 'cc217be2-8270-4819-bc8e-0850c5358872', url: 'https://github.com/Baoth99/SCSS.WebApi.git'
    }
    stage('Restore packages') {
        dotnetRestore project: 'SCSS.WebApi', sdk: 'asp.net5'           
    }
    stage('Clean') {
        dotnetClean project: 'SCSS.WebApi', sdk: 'asp.net5'
    }
    // stage('Increase version') {

    // }
    stage('Build') {
        dotnetBuild project: 'SCSS.WebApi', sdk: 'asp.net5'
    }
    stage('Publish') {
        dotnetPublish project: 'SCSS.WebApi', sdk: 'asp.net5'
    }
}
