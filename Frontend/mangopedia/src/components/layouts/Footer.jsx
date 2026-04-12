export default function Footer() {
  return (
    <footer className="mt-auto py-3 bg-body-tertiary border-top">
      <div className="container small text-muted flex-sm-row text-center ">
        <span>
          &copy; {new Date().getFullYear()} MangoFusion. All rights reserved
        </span>
      </div>
    </footer>
  );
}
