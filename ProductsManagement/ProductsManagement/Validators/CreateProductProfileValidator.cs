using System.Text.RegularExpressions;
using FluentValidation;
using Microsoft.EntityFrameworkCore;
using Microsoft.IdentityModel.Tokens;
using ProductsManagement.Features.Products;
using ProductsManagement.Persistence;
using ProductsManagement.Products;

namespace ProductsManagement.Validators;

public class CreateProductProfileValidator : AbstractValidator<CreateProductProfileRequest>
{
    private readonly ProductsProfileContext _context;
    public CreateProductProfileValidator(ProductsProfileContext context)
    {
        _context = context;
        RuleFor(x => x.Name).NotNull().NotEmpty().WithMessage("Name must not be empty.").MaximumLength(200)
            .MinimumLength(1)
            .Must(BeValidName).MustAsync(async (x, name, cancellationToken) =>
                await BeUniqueName(name, x.Brand, cancellationToken));
        RuleFor(x => x.Brand).NotNull().NotEmpty().MaximumLength(100).MinimumLength(2).Must(BeValidBrandName);
        RuleFor(x => x.Sku).NotNull().NotEmpty().Must(BeValidSku).MustAsync(BeUniqueSku)
            .WithMessage("SKU must be unique.");
        RuleFor(x => x.Category).IsInEnum();
        RuleFor(x => x.Price).GreaterThan(0).LessThan(10000);
        RuleFor(x => x.ReleaseDate).LessThan(DateTime.Now).GreaterThanOrEqualTo(new DateTime(1900, 1, 1));
        RuleFor(x => x.StockQuantity).GreaterThan(0).LessThan(100000);
        RuleFor(x => x.ImageUrl).Must(BeValidImageUrl);
        RuleFor(x => x)
            .MustAsync(async (product, cancellationToken) =>
                await PassBusinessLogic(product));
    }
    
    private static readonly string[] TechnologyKeywords ={
        "tech", "smart", "digital", "electronic", "electronics", "device",
        "wifi", "wi-fi", "internet", "bluetooth", "wireless", "nfc",
        "usb", "type-c", "lightning", "charger", "charging", "cable",
        "battery", "powerbank", "power bank", "power supply", "adapter",
        "headphones", "earbuds", "earphones", "headset", "speaker",
        "soundbar", "subwoofer", "audio", "stereo",

        "laptop", "notebook", "ultrabook", "desktop", "computer", "pc",
        "monitor", "screen", "display", "keyboard", "mouse", "trackpad",
        "webcam", "microphone",

        "gaming", "console", "playstation", "xbox", "nintendo", "vr",
        "virtual reality", "controller", "joystick",

        "camera", "dslr", "mirrorless", "action cam", "gopro",
        "security camera", "surveillance",

        "drone", "quadcopter", "fpv",

        "tablet", "ipad", "android tablet",
        "smartphone", "phone", "mobile", "cellphone",

        "tv", "television", "oled", "led", "qled", "uhd", "4k", "8k",
        "hdmi", "displayport", "vga",

        "ssd", "hard drive", "hdd", "nvme", "storage", "memory", "ram",
        "processor", "cpu", "gpu", "graphics card", "motherboard",
        "router", "modem", "network",

        "projector", "laser projector",

        "smartwatch", "fitness tracker", "wearable",

        "robot", "robotic", "ai", "machine learning", "iot",

        "app", "software", "firmware", "program", "cloud"
    };
    
    private static readonly string[] InappropriateHomeKeywords = {
        "adult", "xxx", "erotic", "porn", "fetish", "nsfw",
        
        "nude", "nudity", "explicit",
        
        "weapon", "gun", "pistol", "rifle", "shotgun", "firearm",
        "knife", "dagger", "blade", "machete", "sword", "crossbow",
        "taser", "stun gun", "explosive", "grenade", "ammo", "bullet",
        "bomb", "firecracker",
        
        "drug", "opioid", "cocaine", "heroin", "meth", "lsd",
        "cannabis", "marijuana", "weed", "hash", "pot",
        "tobacco", "cigarette", "cigar", "vape", "nicotine",
        
        "alcohol", "liquor", "vodka", "whiskey", "beer",
        
        "violent", "violence", "gore", "blood", "bloody", "abuse",
        "assault", "harm",
        
        "racist", "sexist", "hate", "offensive", "profane", "insult",
        
        "lockpick", "lock pick", "hacking", "counterfeit",
        "pirated", "illegal", "unauthorized"
    };
    
    private readonly string[] _validImageExtensions = [".jpg", ".jpeg", ".png", ".gif", ".bmp", ".webp", ".tiff"];
    async Task<bool> BeUniqueName(string name, string brand, CancellationToken cancellation)
    {
        return !await _context.Products
            .Where(e => e.Name == name && e.Brand == brand)
            .AnyAsync(cancellation);
    }
    
    async Task<bool> BeUniqueSku(string sku, CancellationToken cancellation)
    {
        return !await _context.Products
            .Where(e => e.Sku == sku)
            .AnyAsync(cancellation);
    }

    bool BeValidBrandName(string brand)
    {
        string pattern = @"^[a-zA-Z0-9\s\-'\.]+$";
        return Regex.IsMatch(brand, pattern);
    }
    
    bool BeValidSku(string brand)
    {
        string pattern = @"^[a-zA-Z0-9\s\-]+$";
        return Regex.IsMatch(brand, pattern);
    }

    bool BeValidName(string name)
    {
        return InappropriateHomeKeywords.All(t => !name.Contains(t));
    }

    bool BeValidImageUrl(string? url)
    {
        if (url is null)
        {
            return true;
        }

        var isValidUrl = Uri.TryCreate(url, UriKind.Absolute, out var uriResult)
                         && (uriResult.Scheme == Uri.UriSchemeHttp || uriResult.Scheme == Uri.UriSchemeHttps);

        var hasValidExtension = _validImageExtensions.Any(ext => url.EndsWith(ext, StringComparison.OrdinalIgnoreCase));

        return isValidUrl && hasValidExtension;
    }

    async Task<bool> PassBusinessLogic(CreateProductProfileRequest product)
    {
        bool v1 = await ElectronicsPriceCheck(product);
        bool v2 = await HomeProductNameCheck(product.Name);
        bool v3 = await HighValueStockLimit(product);
        return v1 && v2 && v3;
    }
    
    async Task<bool> ElectronicsPriceCheck(CreateProductProfileRequest product)
    {
        if (product.Category != ProductCategory.Electronics) return true;
        return product.Price >= 50;
    }
    
    async Task<bool> HomeProductNameCheck(string name)
    {
        return InappropriateHomeKeywords.All(t => !name.Contains(t));
    }
    
    async Task<bool> HighValueStockLimit(CreateProductProfileRequest product)
    {
        if (product.Price <= 500) return true;
        return product.StockQuantity <= 10;
    }
}