pipeline {
    agent any 

    stages {
        stage('Clean') {
            steps {
              cleanWs()    
            }
        }
        stage('Build') {
            steps {
               echo 'Build'    
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