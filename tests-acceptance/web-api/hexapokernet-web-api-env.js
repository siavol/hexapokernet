const assert = require('node:assert');
const NodeEnvironment = require('jest-environment-node').TestEnvironment;

const waitForMs = delay => new Promise(resolve => setTimeout(resolve, delay));

class HexapokernetWebApiEnvironment extends NodeEnvironment {
    async setup() {
        await super.setup();

        this.global.baseUrl = this.getGlobalBaseUrl();
        this.global.fetch = this.getFetchWithLogging();

        await this.waitAppHealthy();
    }
    
    getGlobalBaseUrl() {
        assert(process.env.APP_HOST, 'Hexapokernet web API host must be set in APP_HOST environment variable');
        assert(process.env.APP_PORT, 'Hexapokernet web API port must be set in APP_PORT environment variable');
        return `http://${process.env.APP_HOST}:${process.env.APP_PORT}`;
    }

    getFetchWithLogging() {
        return function(resource, options) {
            const method = (options && options.method) || 'GET';
            console.log(`[FETCH-REQ] ${method} ${resource}`);
            return global.fetch(resource, options)
                .then(res => {
                    console.log(`[FETCH-RES] HTTP ${res.status}`);
                    return res;
                });
        }
    }
    
    async waitAppHealthy() {
        while (true) {
            const res = await fetch(`${this.global.baseUrl}/health`);
            if (res.ok) {
                return;
            } else {
                console.log(`GET /health returns HTTP ${res.status}. Waiting app to healthy.`);
                await waitForMs(500);
            }
        }
    }
}

module.exports = HexapokernetWebApiEnvironment;