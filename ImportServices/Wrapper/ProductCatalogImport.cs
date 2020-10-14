using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Xml.Serialization;

namespace ImportServices.Wrapper
{
	[XmlRoot(ElementName = "ProductAttributeGroup")]
	public class ProductAttributeGroup
	{
		[XmlElement(ElementName = "ProductAttributeGroupCode")]
		public string ProductAttributeGroupCode { get; set; }
		[XmlElement(ElementName = "ParentAttributeGroupCode")]
		public string ParentAttributeGroupCode { get; set; }
		[XmlElement(ElementName = "Name")]
		public string Name { get; set; }
		[XmlElement(ElementName = "SequenceNo")]
		public string SequenceNo { get; set; }
		[XmlElement(ElementName = "EntityTypeId")]
		public string EntityTypeId { get; set; }
		[XmlElement(ElementName = "ActiveFlag")]
		public string ActiveFlag { get; set; }
		[XmlElement(ElementName = "PrimaryFlag")]
		public string PrimaryFlag { get; set; }
	}

	[XmlRoot(ElementName = "ProductAttributeGroups")]
	public class ProductAttributeGroups
	{
		[XmlElement(ElementName = "ProductAttributeGroup")]
		public ProductAttributeGroup ProductAttributeGroup { get; set; }
	}

	[XmlRoot(ElementName = "ProductAttribute")]
	public class ProductAttribute
	{
		[XmlElement(ElementName = "ProductAttributeCode")]
		public string ProductAttributeCode { get; set; }
		[XmlElement(ElementName = "ProductAttributeGroupCode")]
		public string ProductAttributeGroupCode { get; set; }
		[XmlElement(ElementName = "AttributeName")]
		public string AttributeName { get; set; }
		[XmlElement(ElementName = "SequenceNo")]
		public string SequenceNo { get; set; }
		[XmlElement(ElementName = "ActiveFlag")]
		public string ActiveFlag { get; set; }
		[XmlElement(ElementName = "EntityAttributeType")]
		public string EntityAttributeType { get; set; }
	}

	[XmlRoot(ElementName = "ProductAttributes")]
	public class ProductAttributes
	{
		[XmlElement(ElementName = "ProductAttribute")]
		public List<ProductAttribute> ProductAttribute { get; set; }
	}

	[XmlRoot(ElementName = "Source")]
	public class Source
	{
		[XmlElement(ElementName = "SourceId")]
		public string SourceId { get; set; }
		[XmlElement(ElementName = "SourceCode")]
		public string SourceCode { get; set; }
		[XmlElement(ElementName = "DefaultSourceFlag")]
		public string DefaultSourceFlag { get; set; }
		[XmlElement(ElementName = "SequenceNo")]
		public string SequenceNo { get; set; }
		[XmlElement(ElementName = "ActiveFlag")]
		public string ActiveFlag { get; set; }
	}

	[XmlRoot(ElementName = "Sources")]
	public class Sources
	{
		[XmlElement(ElementName = "Source")]
		public List<Source> Source { get; set; }
	}

	[XmlRoot(ElementName = "Attribute")]
	public class Attribute
	{
		[XmlElement(ElementName = "ProductAttributeCode")]
		public string ProductAttributeCode { get; set; }
		[XmlElement(ElementName = "ProductAttributeValue")]
		public string ProductAttributeValue { get; set; }
	}

	[XmlRoot(ElementName = "Attributes")]
	public class Attributes
	{
		[XmlElement(ElementName = "Attribute")]
		public List<Attribute> Attribute { get; set; }
	}

	[XmlRoot(ElementName = "ProductVariantSource")]
	public class ProductVariantSource
	{
		[XmlElement(ElementName = "SourceId")]
		public string SourceId { get; set; }
		[XmlElement(ElementName = "Price")]
		public string Price { get; set; }
		[XmlElement(ElementName = "OriginalPrice")]
		public string OriginalPrice { get; set; }
		[XmlElement(ElementName = "BackOrderableFlag")]
		public string BackOrderableFlag { get; set; }
		[XmlElement(ElementName = "ActiveFlag")]
		public string ActiveFlag { get; set; }
	}

	[XmlRoot(ElementName = "ProductVariantSources")]
	public class ProductVariantSources
	{
		[XmlElement(ElementName = "ProductVariantSource")]
		public ProductVariantSource ProductVariantSource { get; set; }
	}

	[XmlRoot(ElementName = "ProductVariant")]
	public class ProductVariant
	{
		[XmlElement(ElementName = "Sku")]
		public string Sku { get; set; }
		[XmlElement(ElementName = "ColorName")]
		public string ColorName { get; set; }
		[XmlElement(ElementName = "SizeName")]
		public string SizeName { get; set; }
		[XmlElement(ElementName = "BackOrderableFlag")]
		public string BackOrderableFlag { get; set; }
		[XmlElement(ElementName = "ColorCode")]
		public string ColorCode { get; set; }
		[XmlElement(ElementName = "TaxableFlag")]
		public string TaxableFlag { get; set; }
		[XmlElement(ElementName = "ActiveFlag")]
		public string ActiveFlag { get; set; }
		[XmlElement(ElementName = "ProductVariantSources")]
		public ProductVariantSources ProductVariantSources { get; set; }
		[XmlElement(ElementName = "WeightInLbs")]
		public string WeightInLbs { get; set; }
		[XmlElement(ElementName = "Quantity")]
		public string Quantity { get; set; }
		[XmlElement(ElementName = "Cost")]
		public string Cost { get; set; }
		[XmlElement(ElementName = "TypeId")]
		public string TypeId { get; set; }
	}

	[XmlRoot(ElementName = "ProductVariants")]
	public class ProductVariants
	{
		[XmlElement(ElementName = "ProductVariant")]
		public List<ProductVariant> ProductVariant { get; set; }
	}

	[XmlRoot(ElementName = "Product")]
	public class Product
	{
		[XmlElement(ElementName = "Name")]
		public string Name { get; set; }
		[XmlElement(ElementName = "Attributes")]
		public Attributes Attributes { get; set; }
		[XmlElement(ElementName = "Description")]
		public string Description { get; set; }
		[XmlElement(ElementName = "ActiveFlag")]
		public string ActiveFlag { get; set; }
		[XmlElement(ElementName = "ImageFile")]
		public string ImageFile { get; set; }
		[XmlElement(ElementName = "Style")]
		public string Style { get; set; }
		[XmlElement(ElementName = "DisplayAttribute1")]
		public string DisplayAttribute1 { get; set; }
		[XmlElement(ElementName = "DisplayAttribute2")]
		public string DisplayAttribute2 { get; set; }
		[XmlElement(ElementName = "MetaDescription")]
		public string MetaDescription { get; set; }
		[XmlElement(ElementName = "MetaKeywords")]
		public string MetaKeywords { get; set; }
		[XmlElement(ElementName = "ProductVariants")]
		public ProductVariants ProductVariants { get; set; }
		[XmlElement(ElementName = "SiteId")]
		public string SiteId { get; set; }
		[XmlElement(ElementName = "ProductCode")]
		public string ProductCode { get; set; }
	}

	[XmlRoot(ElementName = "Products")]
	public class Products
	{
		[XmlElement(ElementName = "Product")]
		public List<Product> Product { get; set; }
	}

	[XmlRoot(ElementName = "ProductCatalogImport")]
	public class ProductCatalogImport
	{
		[XmlElement(ElementName = "ProductAttributeGroups")]
		public ProductAttributeGroups ProductAttributeGroups { get; set; }
		[XmlElement(ElementName = "ProductAttributes")]
		public ProductAttributes ProductAttributes { get; set; }
		[XmlElement(ElementName = "Sources")]
		public Sources Sources { get; set; }
		[XmlElement(ElementName = "Products")]
		public Products Products { get; set; }
		[XmlAttribute(AttributeName = "ns2", Namespace = "http://www.w3.org/2000/xmlns/")]
		public string Ns2 { get; set; }
		[XmlAttribute(AttributeName = "ns4", Namespace = "http://www.w3.org/2000/xmlns/")]
		public string Ns4 { get; set; }
		[XmlAttribute(AttributeName = "ns3", Namespace = "http://www.w3.org/2000/xmlns/")]
		public string Ns3 { get; set; }
	}
}