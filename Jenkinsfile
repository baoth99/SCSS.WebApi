node {
    stage('Clean Workspace') {
        cleanWs()    
    
    }
    stage('Git Checkout') {
        git branch: 'develop_ci-cd', credentialsId: 'cc217be2-8270-4819-bc8e-0850c5358872', url: 'https://github.com/Baoth99/SCSS.WebApi.git'
    }
    stage('Restore packages') {
        bat 'dotnet restore "SCSS.WebApi\\SCSS.WebApi.csproj"'
    }
    stage('Build') {
        echo 'Build'    
        bat 'dotnet clean "SCSS.WebApi\\SCSS.WebApi.csproj"'               
        bat 'dotnet build "SCSS.WebApi\\SCSS.WebApi.csproj" -c Release -o /SCSS.WebApi/build'
    }
    stage('Test') {
        steps {
            echo 'Test'    
        }
    }
    stage('Deploy') {
        echo 'Deploy'  
        bat 'dotnet publish "SCSS.WebApi\\SCSS.WebApi.csproj" -c Release -o /SCSS.WebApi/publish'
        bat 'aws --version'
        // withCredentials([aws(accessKeyVariable: 'AWS_ACCESS_KEY_ID', credentialsId: 'fda170f9-93f6-4b08-9099-f3abc7a26f18', secretKeyVariable: 'AWS_SECRET_ACCESS_KEY')]) {
        //     sh 'aws --version'
        // }
        
    }
    
}