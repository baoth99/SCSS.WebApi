pipeline {
    agent {
        node
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