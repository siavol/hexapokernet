const assert = require('node:assert');
const NodeEnvironment = require('jest-environment-node').TestEnvironment;

class HexapokernetWebApiEnvironment extends NodeEnvironment {
    async setup() {
        await super.setup();

        this.setGlobalBaseUrl();
        await this.waitAppHealthy();
    }
    
    setGlobalBaseUrl() {
        assert(process.env.APP_HOST, 'Hexapokernet web API host must be set in APP_HOST environment variable');
        assert(process.env.APP_PORT, 'Hexapokernet web API port must be set in APP_PORT environment variable');
        const baseUrl = `http://${process.env.APP_HOST}:${process.env.APP_PORT}`;
        this.global.baseUrl = baseUrl;
    }
    
    async waitAppHealthy() {
        while (true) {
            const res = await fetch(`${this.global.baseUrl}/health`);
            if (res.ok) {
                return;
            }
        }
    }
}

module.exports = HexapokernetWebApiEnvironment;