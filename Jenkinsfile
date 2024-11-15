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
        NOTIFICATION_EMAIL = 'maulangbonilla.18@gmail.com'
        VAULT_TOKEN = credentials('VAULT_TOKEN')  // Variable de entorno para el token de Vault
    }

    stages {
        stage('Ejecutar Pruebas Unitarias') {
            steps {
                sh 'dotnet test TrackX.Tests/TrackX.Tests.csproj --logger trx'
            }
            post {
                failure {
                    mail to: env.NOTIFICATION_EMAIL,
                         subject: "Error en las pruebas unitarias: ${currentBuild.fullDisplayName}",
                         body: "Las pruebas unitarias han fallado en el build ${env.BUILD_URL}. Verifica el pipeline para más detalles."
                }
            }
        }

        stage('Construcción de Docker') {
            steps {
                script {
                    sh "docker build --build-arg VAULT_TOKEN=${VAULT_TOKEN} -f TrackX.Api/Dockerfile -t ${DOCKER_IMAGE} ."
                }
            }
        }

        stage('Verificar Contenedor de Desarrollo en Ejecución') {
            steps {
                script {
                    def devContainerRunning = sh(script: "docker ps -q -f name=${CONTAINER_NAME_DEV}", returnStdout: true).trim()
                    if (devContainerRunning) {
                        env.DEV_CONTAINER_RUNNING = "true"
                    } else {
                        env.DEV_CONTAINER_RUNNING = "false"
                    }
                }
            }
        }

        stage('Ejecutar Docker (Desarrollo)') {
            when {
                expression { env.GIT_BRANCH == 'origin/develop' }
            }
            steps {
                script {
                    if (env.DEV_CONTAINER_RUNNING == 'true') {
                        echo 'El contenedor de desarrollo ya está en ejecución.'
                    } else {
                        sh "docker run -d -p ${PORT_DEV}:${PORT_CONTAINER} --name ${CONTAINER_NAME_DEV} ${DOCKER_IMAGE}"
                    }
                }
            }
        }

        stage('Docker Compose Up (Producción)') {
            when {
                expression { env.GIT_BRANCH == 'origin/master' }
            }
            steps {
                script {
                    def devContainerRunning = sh(script: "docker ps -q -f name=${CONTAINER_NAME_DEV}", returnStdout: true).trim()
                    if (devContainerRunning) {
                        sh "docker stop ${CONTAINER_NAME_DEV} || true"
                        sh "docker rm ${CONTAINER_NAME_DEV} || true"
                    }
                    dir('/home/administrador') {
                        sh "docker-compose -f ${COMPOSE_NAME} up -d"
                    }
                }
            }
        }
    }

    post {
        always {
            script {
                echo 'Limpiando imágenes de Docker no utilizadas...'
                sh "docker image prune -f"
            }
        }

        success {
            script {
                echo '¡Pipeline completado con éxito!'
                withCredentials([usernamePassword(credentialsId: "${DOCKER_CREDENTIALS_ID}", usernameVariable: 'DOCKER_USER', passwordVariable: 'DOCKER_PASS')]) {
                    sh "docker login -u ${DOCKER_USER} -p ${DOCKER_PASS}"
                    sh "docker push ${DOCKER_IMAGE}"
                }
            }
        }

        failure {
            script {
                echo '¡Pipeline fallido!'
            }
        }
    }
}
