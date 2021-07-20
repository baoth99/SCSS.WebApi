pipeline {
    agent any

    stages {
        stage('Clean Workspace') {
            steps {
              cleanWs()    
            }
        }
        stage('Git Checkout') {
            git branch: 'develop_ci-cd', credentialsId: 'cc217be2-8270-4819-bc8e-0850c5358872', url: 'https://github.com/Baoth99/SCSS.WebApi.git'
        }
        stage('Restore packages') {
            steps {
               dotnetRestore project: 'SCSS.WebApi', sdk: 'asp.net5' 
            }
        }
        stage('Build') {
            steps {
               echo 'Build'    
               dotnetClean project: 'SCSS.WebApi', sdk: 'asp.net5'
            }
        }
        stage('Test') {
            steps {
                echo 'Test'    
            }
        }
        stage('Deploy') {
            steps {
                echo 'Deploy'    
            }
        }
    }
}