pipeline {
    agent any

    tools {
        dotnetsdk 'asp.net5'
        git 'Default'
    }

    stages {
        stage('Clean Workspace') {
            steps {
              cleanWs()    
            }
        }
        stage('Git Checkout') {
            steps {
                git branch: 'develop_ci-cd', credentialsId: 'cc217be2-8270-4819-bc8e-0850c5358872', url: 'https://github.com/Baoth99/SCSS.WebApi.git'
            }
        }
        stage('Restore packages') {
            steps {
               dotnet restore "SCSS.WebApi.csproj"
            }
        }
        stage('Build') {
            steps {
               echo 'Build'    
               dotnet clean "SCSS.WebApi"
               dotnet build "SCSS.WebApi.csproj" -c Release -o /SCSS.WebApi/build
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