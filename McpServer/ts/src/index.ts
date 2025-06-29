import { Server } from "@modelcontextprotocol/sdk/server/index.js";
import { StdioServerTransport } from "@modelcontextprotocol/sdk/server/stdio.js";
import { CallToolRequestSchema, ListToolsRequestSchema } from "@modelcontextprotocol/sdk/types.js";
import z from "zod";
import { quoteOfTheDayResponse } from "./quoteOfTheDay.js";

const server = new Server({
  name: "mcp-server",
  version: "1.0.0",
}, {
  capabilities: {
    tools: {}
  }
});

// Register tools
server.setRequestHandler(ListToolsRequestSchema, async () => {
  return {
    tools: [
      {
        name: "get_cocktail",
        description: "Search for cocktail recipes by name",
        inputSchema: {
          type: "object",
          properties: {
            name: {
              type: "string",
              description: "Cocktail name to search for"
            }
          },
          required: ["name"]
        }
      },
      {
        name: "get_quote_of_the_day",
        description: "Retrieve a random quote of the day",
        inputSchema: {
          type: "object",
          properties: {},
          required: []
        }
      }
    ]
  };
});

// Define tool schema using zod
const getCocktailSchema = z.object({
  name: z.string().describe("Cocktail name to search for")
});

server.setRequestHandler(CallToolRequestSchema, async (request) => {
  if (request.params.name === "get_cocktail") {
    try {
      // Parse and validate arguments using zod
      const args = getCocktailSchema.parse(request.params.arguments);

      // Make the API call to CocktailDB
      const url = `
       https://www.thecocktaildb.com/api/json/v1/1/search.php?
       s=${encodeURIComponent(args.name)}
      `;
      const response = await fetch(url);

      if (!response.ok) {
        throw new Error(`CocktailDB API error: ${response.statusText}`);
      }

      const data = await response.json();

      // Check if any drinks were found
      if (!data.drinks) {
        return {
          content: [
            {
              type: "text",
              text: `
               No cocktails found matching "${args.name}". 
               Try a different search term.
              `
            }
          ]
        };
      }

      // Format each cocktail recipe
      const cocktailRecipes = data.drinks.map(formatCocktail);

      // Create the formatted response
      const result = `
        Found ${data.drinks.length} cocktail(s) matching 
        "${args.name}":\n\n${cocktailRecipes.join('\n\n')}
      `;

      return {
        content: [
          {
            type: "text",
            text: result
          }
        ]
      };
    } catch (error) {
      console.error("Error in get_cocktail tool:", error);

      return {
        isError: true,
        content: [
          {
            type: "text",
            text: `Error searching for cocktail: 
${error instanceof Error ? error.message : 'Unknown error'}`
          }
        ]
      };
    }
  }  

  if (request.params.name === "get_quote_of_the_day") {
    // Return the quote of the day response
    return quoteOfTheDayResponse;
  }

  // Handle unknown tool
  return {
    isError: true,
    content: [
      {
        type: "text",
        text: `Unknown tool: ${request.params.name}`
      }
    ]
  };
});

// ... rest of the code
function formatCocktail(drink: any) {
  // Create an array of ingredients paired with measurements
  const ingredients = [];
  for (let i = 1; i <= 15; i++) {
    const ingredient = drink[`strIngredient${i}`];
    const measure = drink[`strMeasure${i}`];

    if (ingredient) {
      ingredients.push(`${measure ? measure.trim() : ''} ${ingredient}`);
    }
  }

  return `
🍸 ${drink.strDrink} 🍸
-----------------
Category: ${drink.strCategory}
Glass: ${drink.strGlass}
Alcoholic: ${drink.strAlcoholic}

Ingredients:
${ingredients.map(i => `• ${i.trim()}`).join('\n')}

Instructions:
${drink.strInstructions}
  `.trim();
}

const transport = new StdioServerTransport();
await server.connect(transport);