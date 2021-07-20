pipeline {
    agent any 

    stages {
        stage('Clean') {
            steps {
              cleanWs()    
            }
        }
        stage('Restore packages') {
            steps {
                sh 'dotnet restore SCSS.WebApi.sln'         
            }
        }
    }
}