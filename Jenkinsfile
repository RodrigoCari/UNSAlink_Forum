pipeline {
    agent any

    environment {
        DOTNET_CLI_TELEMETRY_OPTOUT = '1'
        SONAR_TOKEN = credentials('sonar-token') // Assumed credential
    }

    stages {
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
                    // Start Backend in background
                    bat 'start /B dotnet run --project ForoUniversitario/ForoUniversitario.csproj --urls "http://localhost:5000"'
                    
                    // Start Frontend in background
                    dir('Frontend') {
                        bat 'start /B npm run preview -- --port 5173'
                    }

                    // Wait for services to start
                    sleep 15

                    echo 'Running OWASP ZAP scan...'
                    // Run ZAP Quick Scan
                    // Note: Using "call" to ensure bat execution doesn't exit immediately if it's a script
                    // -cmd: Run in command line mode
                    // -quickurl: The URL to scan
                    // -quickout: Output path for the report
                    def zapPath = '"C:\\Program Files\\ZAP\\Zed Attack Proxy\\zap.bat"'
                    def reportPath = "${WORKSPACE}\\zap-report.html"
                    
                    try {
                        bat "${zapPath} -cmd -quickurl http://localhost:5173 -quickout \"${reportPath}\""
                    } catch (Exception e) {
                        echo "ZAP Scan encountered issues: ${e.message}"
                    } finally {
                        echo 'Stopping services...'
                        // Kill the processes started. This is a rough cleanup for Windows.
                        // Warning: This might kill other dotnet/node instances if running.
                        bat 'taskkill /F /IM dotnet.exe /T || exit 0'
                        bat 'taskkill /F /IM node.exe /T || exit 0'
                    }
                }
            }
        }

        stage('Deploy') {
            steps {
                script {
                    echo 'Deploying Application (Live Execution)...'
                    
                    // 1. Kill existing processes (Clean Slate)
                    // Note: This is aggressive and kills ALL dotnet/node processes. 
                    // In a shared environment, you would need more specific filters.
                    try {
                        bat 'taskkill /F /IM dotnet.exe /T || exit 0'
                        bat 'taskkill /F /IM node.exe /T || exit 0'
                    } catch (Exception e) {
                        echo 'No processes to kill.'
                    }

                    // 2. Start Backend (Detached)
                    // JENKINS_NODE_COOKIE=dontKillMe tells Jenkins NOT to kill this process tree
                    withEnv(['JENKINS_NODE_COOKIE=dontKillMe']) {
                        echo 'Starting Backend on http://localhost:5000...'
                        bat 'start /B dotnet run --project ForoUniversitario/ForoUniversitario.csproj --urls "http://localhost:5000"'
                    }

                    // 3. Start Frontend (Detached)
                    withEnv(['JENKINS_NODE_COOKIE=dontKillMe']) {
                        dir('Frontend') {
                            echo 'Starting Frontend on http://localhost:5173...'
                            bat 'start /B npm run dev -- --port 5173'
                        }
                    }

                    // 4. Wait for startup
                    sleep 10
                    echo 'Deployment Complete. Backend: http://localhost:5000, Frontend: http://localhost:5173'
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
