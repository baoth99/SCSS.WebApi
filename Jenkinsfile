pipeline {
    agent any

    environment {
        dotnet = 'path\\to\\dotnet.exe'
    }
    stages {
        stage('Clean Workspace') {
            steps {
              cleanWs()    
            }
        }
        stage('Build') {
            steps {
               echo 'Build'    
               sh 'dotnet restore SCSS.WebApi.sln'
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