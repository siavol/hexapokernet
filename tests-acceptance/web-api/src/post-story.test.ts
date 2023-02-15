/**
 * @jest-environment ./hexapokernet-web-api-env
 */
import {describe, expect, test, beforeAll} from '@jest/globals';

describe('POST /story', () => {
    let baseUrl: string;
    let fetch: typeof global.fetch;

    beforeAll(() => {
        baseUrl = globalThis.baseUrl;
        fetch = globalThis.fetch;
    });
    
    describe('When requesting with valid JSON body', () => {
        let res: Response;
        
        beforeAll(async () => {
            res = await fetch(`${baseUrl}/story`, {
                method: 'POST',
                headers: {
                    'content-type': 'application/json'
                },
                body: JSON.stringify({ title: 'Test story' })
            });
        });
        
        test('should respond with HTTP 200', () => {
            expect(res.status).toBe(200);
        });
        
        test('should respond with body having story id', async () => {
            expect(await res.json()).toEqual({
                id: expect.any(String)
            });
        });
    });
    
    describe('When requesting with incorrect "content-type" header value', () => {
        let res: Response;

        beforeAll(async () => {
            res = await fetch(`${baseUrl}/story`, {
                method: 'POST',
                body: JSON.stringify({ title: 'Test story' })
            });
        });

        test('should respond with HTTP 415 Unsupported Media Type', () => {
            expect(res.status).toBe(415);
        });
    });

    describe('When requesting with incorrect JSON in the body', () => {
        let res: Response;

        beforeAll(async () => {
            res = await fetch(`${baseUrl}/story`, {
                method: 'POST',
                headers: {
                    'content-type': 'application/json'
                },
                body: JSON.stringify({ foo: 'wtf' })
            });
        });

        test('should respond with HTTP 400 Bad Request', () => {
            expect(res.status).toBe(400);
        });
    });
});
