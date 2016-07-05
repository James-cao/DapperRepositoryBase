-- Database: catalog

-- DROP DATABASE catalog;

CREATE DATABASE catalog
  WITH OWNER = postgres
       ENCODING = 'UTF8'
       TABLESPACE = pg_default
       LC_COLLATE = 'Spanish_Dominican Republic.1252'
       LC_CTYPE = 'Spanish_Dominican Republic.1252'
       CONNECTION LIMIT = -1;


-- Table: public.product
-- DROP TABLE public.product;

CREATE TABLE public.product
(
  name text NOT NULL,
  id integer NOT NULL DEFAULT nextval('product_id_seq'::regclass),
  description text,
  isenable boolean,
  lastupdate timestamp with time zone,
  prices money
)
WITH (
  OIDS=FALSE
);
ALTER TABLE public.product
  OWNER TO postgres;
