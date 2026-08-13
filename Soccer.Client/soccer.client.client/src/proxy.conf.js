const PROXY_CONFIG = [
  {
    context: ['/api'],
    target: 'https://127.0.0.1:7030',
    changeOrigin: true,
    logLevel: 'debug'
  }
];

module.exports = PROXY_CONFIG;
