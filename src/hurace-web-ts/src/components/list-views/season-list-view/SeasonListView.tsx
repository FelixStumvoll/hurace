import React from 'react';
import { SeasonListViewItem } from './SeasonListViewItem';
import { Season } from '../../../models/Season';
import { getSeasons } from '../../../common/api';
import { ListViewWrapper, SearchContext } from '../../shared/ListViewWrapper';
import { useStateAsync } from '../../../hooks/useStateAsync';
import { FlexWrap } from '../../../theme/CustomComponents';

const seasonFilter = (season: Season, searchTerm: string) =>
    season.startDate
        .getFullYear()
        .toString()
        .toLowerCase()
        .includes(searchTerm.toLowerCase()) ||
    season.endDate
        .getFullYear()
        .toString()
        .toLowerCase()
        .includes(searchTerm.toLowerCase());

export const SeasonListView: React.FC = () => {
    const [seasons] = useStateAsync(getSeasons);

    return (
        <ListViewWrapper
            createConfig={{
                createText: 'Saison erstellen',
                createUrl: '/season/new'
            }}
        >
            <SearchContext.Consumer>
                {searchTerm => (
                    <FlexWrap>
                        {seasons
                            ?.filter(s => seasonFilter(s, searchTerm))
                            .map(s => (
                                <SeasonListViewItem key={s.id} season={s} />
                            ))}
                    </FlexWrap>
                )}
            </SearchContext.Consumer>
        </ListViewWrapper>
    );
};
