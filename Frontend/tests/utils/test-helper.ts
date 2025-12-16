import {Page, Request} from "@playwright/test";
import {environment} from "../../src/environments/environment.development";

const apiUrl = environment.apiUrl;

export function captureApiRequests(page: Page) {
  const requests: Request[] = [];

  page.on('request', request => {
    if (request.url().includes(apiUrl)) {
      requests.push(request);
    }
  });

  return requests;
}
