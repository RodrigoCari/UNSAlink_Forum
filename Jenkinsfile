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
                    sh 'dotnet restore'
                    sh 'dotnet build --configuration Release --no-restore'
                }
            }
        }

        stage('Build Frontend') {
            steps {
                dir('Frontend') {
                    sh 'npm install'
                    sh 'npm run build'
                }
            }
        }

        stage('Unit Tests') {
            steps {
                dir('ForoUniversitario.Tests') {
                    sh 'dotnet test --configuration Release --no-build --collect:"XPlat Code Coverage"'
                }
            }
        }

        stage('Static Analysis') {
            steps {
                dir('ForoUniversitario') {
                    // Assuming SonarScanner for .NET is installed globally or available
                    sh 'dotnet sonarscanner begin /k:"ForoUniversitario" /d:sonar.host.url="http://localhost:9000" /d:sonar.login="${SONAR_TOKEN}"'
                    sh 'dotnet build'
                    sh 'dotnet sonarscanner end /d:sonar.login="${SONAR_TOKEN}"'
                }
            }
        }

        stage('Functional Tests') {
            steps {
                dir('tests/functional') {
                    // Assumes python and selenium are available
                    sh 'pip install selenium'
                    sh 'python functional_tests.py'
                }
            }
        }

        stage('Performance Tests') {
            steps {
                dir('tests/performance') {
                    // Assumes jmeter is available in path
                    sh 'jmeter -n -t performance_plan.jmx -l results.jtl'
                }
            }
        }

        stage('Security Tests') {
            steps {
                // Placeholder for OWASP ZAP
                echo 'Running OWASP ZAP scan...'
                // sh 'zap-cli quick-scan http://localhost:5000'
            }
        }
    }

    post {
        always {
            junit '**/TestResults/*.xml'
        }
    }
}
