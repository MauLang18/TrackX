pipeline {
    agent any

    environment {
        DOCKER_IMAGE = 'maulang18/trackx.api:latest'
        CONTAINER_NAME_DEV = 'TrackXCFDev'
        PORT_DEV = '10108'
        PORT_CONTAINER = '8080'
        COMPOSE_NAME = '/home/administrador/docker-compose-castrofallas.yml'
        DOCKER_CREDENTIALS_ID = 'dockerhub-credentials-id'
        SONARQUBE_TOKEN = credentials('Sonar-Token')
        SONARQUBE_HOST_URL = 'http://20.81.187.2:9000'
        SONARQUBE_PROJECT_KEY = 'TrackX-CastroFallas'
        PATH = "${PATH}:/home/administrador/.dotnet/tools"
    }

    stages {
        // stage('Check Dev Container Running') {
        //     steps {
        //         script {
        //             def devContainerRunning = sh(script: "docker ps -q -f name=${CONTAINER_NAME_DEV}", returnStdout: true).trim()
        //             if (devContainerRunning) {
        //                 env.DEV_CONTAINER_RUNNING = "true"
        //             } else {
        //                 env.DEV_CONTAINER_RUNNING = "false"
        //             }
        //         }
        //     }
        // }

        // stage('Docker Build') {
        //     when {
        //         expression { env.DEV_CONTAINER_RUNNING == 'false' }
        //     }
        //     steps {
        //         script {
        //             sh "docker build -f TrackX.Api/Dockerfile -t ${DOCKER_IMAGE} ."
        //         }
        //     }
        // }

        stage('SonarQube Analysis') {
            steps {
                script {
                    echo "PATH: ${env.PATH}"
                    withSonarQubeEnv('Sonar-Server') {
                    sh 'dotnet sonarscanner begin /k:TrackX-CastroFallas /d:sonar.host.url=$SONARQUBE_URL /d:sonar.login=$SONARQUBE_TOKEN /d:sonar.cs.opencover.reportsPaths=**/coverage.opencover.xml'
                    sh 'dotnet build TrackX.sln'
                    sh 'dotnet sonarscanner end /d:sonar.login=$SONARQUBE_TOKEN'
                }
                }
            }
        }

        // stage('Docker Run (Development)') {
        //     when {
        //         expression { env.GIT_BRANCH == 'origin/develop' }
        //     }
        //     steps {
        //         script {
        //             if (env.DEV_CONTAINER_RUNNING == 'true') {
        //                 echo 'Development container is already running.'
        //             } else {
        //                 sh "docker run -d -p ${PORT_DEV}:${PORT_CONTAINER} --name ${CONTAINER_NAME_DEV} ${DOCKER_IMAGE}"
        //             }
        //         }
        //     }
        // }

        // stage('Docker Compose Up (Production)') {
        //     when {
        //         expression { env.GIT_BRANCH == 'origin/master' }
        //     }
        //     steps {
        //         script {
        //             def devContainerRunning = sh(script: "docker ps -q -f name=${CONTAINER_NAME_DEV}", returnStdout: true).trim()
        //             if (devContainerRunning) {
        //                 sh "docker stop ${CONTAINER_NAME_DEV} || true"
        //                 sh "docker rm ${CONTAINER_NAME_DEV} || true"
        //             }
        //             dir('/home/administrador') {
        //                 sh "docker-compose -f ${COMPOSE_NAME} up -d"
        //             }
        //         }
        //     }
        // }
    }

    post {
        always {
            script {
                echo 'Cleaning up unused Docker images...'
                sh "docker image prune -f"
            }
        }

        success {
            script {
                echo 'Pipeline succeeded!'
                // withCredentials([usernamePassword(credentialsId: "${DOCKER_CREDENTIALS_ID}", usernameVariable: 'DOCKER_USER', passwordVariable: 'DOCKER_PASS')]) {
                //     sh "docker login -u ${DOCKER_USER} -p ${DOCKER_PASS}"
                //     sh "docker push ${DOCKER_IMAGE}"
                // }
            }
        }

        failure {
            script {
                echo 'Pipeline failed!'
            }
        }
    }
}
