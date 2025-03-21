// pipeline {
//     agent any

//     environment {
//         DOTNET_CLI_TELEMETRY_OPTOUT = '1'
//     }

//     stages {
//         stage('Clone Repository') {
//             steps {
//                 echo 'Cloning Git repository...'
//                 git branch: 'main', url: 'https://github.com/kanupindi14/ArgusRestaurentCheckoutAutomation.git'
//             }
//         }

//         stage('Restore') {
//             steps {
//                 echo 'Restoring NuGet packages...'
//                 bat 'dotnet restore'
//             }
//         }

//         stage('Build') {
//             steps {
//                 echo 'Building the project...'
//                 bat 'dotnet build --configuration Release'
//             }
//         }

//         stage('Test') {
//             steps {
//                 echo 'Running SpecFlow tests...'
//                 bat 'dotnet test --no-build --verbosity normal'
//             }
//         }
//     }

//     post {
//         always {
//             echo 'Pipeline finished.'
//         }
//         success {
//             echo '✅ Build and test succeeded.'
//         }
//         failure {
//             echo '❌ Build or tests failed.'
//         }
//     }
// }
