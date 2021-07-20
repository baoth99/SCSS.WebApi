pipeline {
    agent any

    stages {
        stage('Clean workspace') {
            steps {
                cleanWs()
            }
        }
        stage('Git Checkout') {
            steps {
                steps {
                    git branch: 'develop_ci-cd', credentialsId: 'cc217be2-8270-4819-bc8e-0850c5358872', url: 'https://github.com/Baoth99/SCSS.WebApi.git'
                }
            }
        }
        stage('Restore packages') {
            steps {
                bat "dotnet restore SCSS.WebApi.sln"
            }
        }
        stage('Clean') {
            steps {
                bat "msbuild.exe SCSS.WebApi.sln" /nologo /nr:false /p:platform=\"x64\" /p:configuration=\"release\" /t:clean"
            }
        }
    }
}
