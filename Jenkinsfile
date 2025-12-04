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
                    bat 'dotnet test --configuration Release --no-build --logger "junit;LogFilePath=../TestResults/test-results.xml" --collect:"XPlat Code Coverage"'
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
                dir('tests/functional') {
                    // Assumes python and selenium are available
                    bat 'pip install selenium'
                    bat 'python functional_tests.py'
                }
            }
        }

        stage('Performance Tests') {
            steps {
                dir('tests/performance') {
                    // Assumes jmeter is available in path
                    bat 'jmeter -n -t performance_plan.jmx -l results.jtl'
                }
            }
        }

        stage('Security Tests') {
            steps {
                // Placeholder for OWASP ZAP
                echo 'Running OWASP ZAP scan...'
                // bat 'zap-cli quick-scan http://localhost:5000'
            }
        }
    }

    post {
        always {
            junit '**/TestResults/*.xml'
        }
    }
}
