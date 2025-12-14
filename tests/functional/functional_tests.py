import unittest
from selenium import webdriver
from selenium.webdriver.chrome.options import Options

class FunctionalTests(unittest.TestCase):
    def setUp(self):
        chrome_options = Options()
        chrome_options.add_argument("--headless") 
        chrome_options.add_argument("--no-sandbox")
        self.driver = webdriver.Chrome(options=chrome_options)

    def test_homepage_title(self):
        self.driver.get("http://localhost:5173") # Assuming frontend runs here
        self.assertIn("UNSAlink", self.driver.title)

    def tearDown(self):
        self.driver.quit()

if __name__ == "__main__":
    unittest.main()
