/**
 * @jest-environment ./hexapokernet-web-api-env
 */
import {describe, expect, test, beforeAll} from '@jest/globals';

describe('GET /story/{storyId}', () => {
    let baseUrl: string;

    beforeAll(() => {
        baseUrl = globalThis.baseUrl;
    });

    describe('When requesting not-existing story', () => {
        let res: Response;

        beforeAll(async () => {
            const notExistingId = 'wat';
            res = await fetch(`${baseUrl}/story/${notExistingId}`);
        });

        test('should respond with HTTP 404 Not Found', () => {
            expect(res.status).toBe(404);
        });
    });
    
    describe('When requesting existing story', () => {
        let storyId: string;
        let res: Response;
        
        beforeAll(async () => {
            const res = await fetch(`${baseUrl}/story`, {
                method: 'POST',
                headers: {
                    'content-type': 'application/json'
                },
                body: JSON.stringify({ title: 'Test story' })
            });
            expect(res.ok);
            
            const body = await res.json();
            storyId = body.id;
        });

        beforeAll(async () => {
            res = await fetch(`${baseUrl}/story/${storyId}`);
        });

        test('should respond with HTTP 200 OK', () => {
            expect(res.status).toBe(200);
        });

        test('should respond with body having story details', async () => {
            expect(await res.json()).toEqual({
                id: storyId,
                title: 'Test story'
            });
        });
    });
});
