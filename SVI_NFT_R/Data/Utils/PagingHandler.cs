using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;

public sealed class PagingHandler<TItem>
{
    public IList<TItem> Items => mItems;
    public IReadOnlyList<TItem> PageItems { get; private set; }
    public int PageCount { get; private set; }
    public int PageIndex
    {
        get
        {
            return mPageIndex;
        }
        set
        {
            SetPageIndex(value);
        }
    }
    public int PageDisplayItemCount
    {
        get
        {
            return mPageDisplayItemCount;
        }
        set
        {
            SetPageDisplayItemCount(value);
        }
    }
    public event EventHandler OnPageIndexChanged;
    private int mPageIndex = -1;
    private int mPageDisplayItemCount = -1;
    private readonly ObservableCollection<TItem> mItems;
    private bool mbIsLockUpdate = false;

    public PagingHandler(IEnumerable<TItem> items, int pageDisplayItemCount)
    {
        mItems = new ObservableCollection<TItem>(items);
        mItems.CollectionChanged += Items_CollectionChanged;
        SetPageDisplayItemCount(pageDisplayItemCount, bForcedUpdate: true);
    }

    public void LockUpdate()
    {
        mbIsLockUpdate = true;
    }

    public void ReleaseUpdate()
    {
        mbIsLockUpdate = false;
    }

    public int SetFirstPage(bool bForcedUpdate = false)
    {
        return SetPageIndex(0, bForcedUpdate);
    }

    public int SetPreviousPage(bool bForcedUpdate = false)
    {
        return SetPageIndex(mPageIndex - 1, bForcedUpdate);
    }

    public int SetNextPage(bool bForcedUpdate = false)
    {
        return SetPageIndex(mPageIndex + 1, bForcedUpdate);
    }

    public int SetLastPage(bool bForcedUpdate = false)
    {
        return SetPageIndex(PageCount - 1, bForcedUpdate);
    }

    public override string ToString()
    {
        return $"{mPageIndex + 1} / {PageCount}";
    }

    private void SetPageDisplayItemCount(int setPageDisplayItemCount, bool bForcedUpdate = false)
    {
        if (setPageDisplayItemCount < 1)
        {
            setPageDisplayItemCount = 1;
        }

        if (bForcedUpdate == false
            && setPageDisplayItemCount == mPageDisplayItemCount
            )
        {
            return;
        }

        mPageDisplayItemCount = setPageDisplayItemCount;
        PageCount = Items.Count / mPageDisplayItemCount + (Items.Count % mPageDisplayItemCount == 0 ? 0 : 1);

        SetPageIndex(mPageIndex, bForcedUpdate);
    }

    private int SetPageIndex(int setPageIndex, bool bForcedUpdate = false)
    {
        if (setPageIndex < 0)
        {
            setPageIndex = 0;
        }
        else if (setPageIndex >= PageCount)
        {
            setPageIndex = PageCount - 1;
        }

        if (bForcedUpdate == false
            && mPageIndex == setPageIndex
            )
        {
            return mPageIndex;
        }

        mPageIndex = setPageIndex;
        PageItems = Items.Skip(mPageIndex * mPageDisplayItemCount)
            .Take(mPageDisplayItemCount)
            .ToList();
        RaiseOnPageChangedEvent();
        return mPageIndex;
    }

    private void RaiseOnPageChangedEvent()
    {
        OnPageIndexChanged?.Invoke(this, EventArgs.Empty);
    }

    private void Items_CollectionChanged(object sender, System.Collections.Specialized.NotifyCollectionChangedEventArgs e)
    {
        if (mbIsLockUpdate == true)
        {
            return;
        }
        SetPageDisplayItemCount(mPageDisplayItemCount, bForcedUpdate: true);
    }
}