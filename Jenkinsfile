pipeline {
    agent any

    environment {
        DOTNET_CLI_TELEMETRY_OPTOUT = '1'
        SONAR_TOKEN = credentials('sonar-token') // Assumed credential
    }

    stages {
        stage('Clean Environment') {
            steps {
                script {
                    echo 'Cleaning up previous processes and Docker containers...'
                    // Stop Docker containers
                    try {
                        bat 'docker compose down --remove-orphans || exit 0'
                    } catch (Exception e) {
                        echo 'No Docker containers to stop.'
                    }
                    try {
                        bat 'taskkill /F /IM dotnet.exe /T || exit 0'
                        bat 'taskkill /F /IM node.exe /T || exit 0'
                    } catch (Exception e) {
                        echo 'No processes to kill.'
                    }
                }
            }
        }

        stage('Checkout') {
            steps {
                checkout scm
            }
        }

        stage('Build Backend') {
            steps {
                dir('ForoUniversitario') {
                    bat 'dotnet restore'
                    bat 'dotnet build --configuration Release --no-restore'
                }
            }
        }

        stage('Docker Build') {
            steps {
                script {
                    echo 'Building Docker images for backend...'
                    bat 'docker compose build backend'
                }
            }
        }

        stage('Build Frontend') {
            steps {
                dir('Frontend') {
                    bat 'npm install'
                    bat 'npm run build'
                }
            }
        }

        stage('Unit Tests') {
            steps {
                dir('ForoUniversitario.Tests') {
                    bat 'dotnet test --configuration Release --logger "junit;LogFilePath=..\\test-results.xml" --collect:"XPlat Code Coverage"'
                }
            }
        }

        stage('Static Analysis') {
            steps {
                dir('ForoUniversitario') {
                    // Assuming SonarScanner for .NET is installed globally or available
                    bat 'dotnet sonarscanner begin /k:"ForoUniversitario" /d:sonar.host.url="http://localhost:9000" /d:sonar.login="%SONAR_TOKEN%"'
                    bat 'dotnet build'
                    bat 'dotnet sonarscanner end /d:sonar.login="%SONAR_TOKEN%"'
                }
            }
        }

        stage('Functional Tests') {
            steps {
                dir('Frontend') {
                    // Start frontend in background, wait 10s, then run tests
                    bat 'start /B npm run preview -- --port 5173 & timeout /t 10 & cd ..\\tests\\functional & pip install selenium & python functional_tests.py'
                }
            }
        }

        stage('Performance Tests') {
            steps {
                dir('tests/performance') {
                    // Assumes jmeter is available in path
                    bat '"C:\\Jmeter\\apache-jmeter-5.6.3\\bin\\jmeter.bat" -n -t performance_plan.jmx -l results.jtl'
                }
            }
        }

        stage('Security Tests') {
            steps {
                script {
                    echo 'Starting services for Security Testing...'
                    // Force stop any existing containers to avoid port conflicts
                    bat 'docker compose down --volumes --remove-orphans || exit 0'
                    // Start Backend via Docker with force recreate
                    bat 'docker compose up -d --force-recreate'
                    
                    // Start Frontend in background
                    dir('Frontend') {
                        bat 'start /B npm run preview -- --port 5173'
                    }

                    // Wait for services to start
                    sleep 20

                    echo 'Running OWASP ZAP scan...'
                    def zapPath = '"C:\\Program Files\\ZAP\\Zed Attack Proxy\\zap.bat"'
                    def reportPath = "${WORKSPACE}\\zap-report.html"
                    
                    try {
                        bat "${zapPath} -cmd -quickurl http://localhost:5173 -quickout \"${reportPath}\""
                    } catch (Exception e) {
                        echo "ZAP Scan encountered issues: ${e.message}"
                    } finally {
                        echo 'Stopping services...'
                        bat 'docker compose down || exit 0'
                        bat 'taskkill /F /IM node.exe /T || exit 0'
                    }
                }
            }
        }

        stage('Deploy') {
            steps {
                script {
                    echo 'Deploying Application (Docker Backend + Local Frontend)...'
                    
                    // 1. Stop existing Docker containers and node processes
                    bat 'docker compose down --volumes --remove-orphans || exit 0'
                    try {
                        bat 'taskkill /F /IM node.exe /T || exit 0'
                    } catch (Exception e) {
                        echo 'No node processes to kill.'
                    }

                    // 2. Start Backend via Docker (detached)
                    echo 'Starting Backend via Docker on http://localhost:5000...'
                    bat 'docker compose up -d --force-recreate'

                    // 3. Start Frontend (Detached)
                    withEnv(['JENKINS_NODE_COOKIE=dontKillMe', 'VITE_API_BASE=http://localhost:5000/api']) {
                        dir('Frontend') {
                            echo 'Starting Frontend on http://localhost:5173 with API at http://localhost:5000/api...'
                            bat 'start /B npm run dev -- --port 5173'
                        }
                    }

                    // 4. Wait for startup and verify
                    sleep 15
                    echo 'Verifying Docker containers are running...'
                    bat 'docker compose ps'
                    echo 'Deployment Complete. Backend (Docker): http://localhost:5000, Frontend: http://localhost:5173'
                }
            }
        }
    }

    post {
        always {
            junit 'test-results.xml'
        }
    }
}
