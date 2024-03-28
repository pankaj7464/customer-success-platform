
  private showLoader(): void {
    this.loading = true;
  }

  private hideLoader(): void {
    this.loading = false;
  }

  isLoading(){
    return this.loading;
  }

  private handleLoader<T>(): (source: Observable<T>) => Observable<T> {
    return (source: Observable<T>) => {
      return source.pipe(
        catchError(error => {
          this.hideLoader();
          return this.handleError(error);
        }),
        finalize(() => {
          this.hideLoader();
        })
      );
    };
  }