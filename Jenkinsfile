pipeline {
    agent any 

    stages {
        stage('Clean Workspace') {
            steps {
              cleanWs()    
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