import {expect, test} from "@playwright/test";
import {captureApiRequests} from "./utils/test-helper";


test('Home page renders correctly', async ({page}) => {
  const requests = captureApiRequests(page);

  await page.goto('/');

  const expectedRequests = [
    "/Order",
    "/Order/latest",
    "/Order/orders-count",
    "/Order/boxes-sold",
    "Stats"
  ]

  await page.waitForLoadState("networkidle");

  expect(requests.length).toBe(expectedRequests.length);

  for (const request of requests) {
    const response = await request.response();
    expect(response).not.toBeNull();
    expect(response!.status()).toBe(200);
  }
});
