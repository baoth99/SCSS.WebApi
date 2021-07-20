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
                sh 'dotnetRestore project: 'SCSS.WebApi', sdk: 'asp.net5''         
            }
        }
    }
}