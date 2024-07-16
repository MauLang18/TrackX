pipeline {
    agent any

    stages {
        stage('Debug') {
            steps {
                script {
                    echo "Current branch: ${env.BRANCH_NAME}"
                }
            }
        }
        /*stage('Docker Build') {
            steps {
                script {
                    bat "docker build -f TrackX.Api/Dockerfile -t maulang18/trackx:latest ."
                }
            }
        }
        stage('Docker Run (Development)') {
            when {
                branch 'develop'
            }
            steps {
                script {
                    bat '''
                        docker stop TrackXDev || true
                        docker rm TrackXDev || true
                    '''
                    bat 'docker run -d -p 10108:8080 --name TrackXDev maulang18/trackx:latest'
                }
            }
        }
        stage('Docker Compose Up (Production)') {
            when {
                branch 'master'
            }
            steps {
                script {
                    bat '''
                        docker stop TrackXDev || true
                        docker rm TrackXDev || true
                    '''
                    dir('C:/Users/administrador/Desktop/Docker/CastroFallas') {
                        bat 'docker-compose up -d'
                    }
                }
            }
        }*/
    }

    post {
        /*always {
            script {
                echo 'Cleaning up...'
                bat 'docker system prune -af'
            }
        }*/

        success {
            script {
                echo 'Pipeline succeeded!'
            }
        }

        failure {
            script {
                echo 'Pipeline failed!'
            }
        }
    }
}
