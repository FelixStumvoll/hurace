import React from 'react';
import styled from 'styled-components';
import { Skier } from '../../models/Skier';
import { getSkiers } from '../../common/api';
import { SkierListViewItem } from './SkierListViewItem';
import { ListViewWrapper } from '../shared/ListViewWrapper';
import { SearchContext } from '../shared/ListViewWrapper';
import { useAsync } from 'react-async-hook';
import { LoadingWrapper } from '../shared/LoadingWrapper';

const SkierList = styled.div`
    display: flex;
    flex-wrap: wrap;
    justify-content: center;
    overflow: auto;
`;

const ListItemWrapper = styled.div`
    margin: 0 ${props => props.theme.gap} ${props => props.theme.gap} 0;
`;

const skierFilter = (skier: Skier, searchTerm: string): boolean =>
    skier.firstName.toLowerCase().includes(searchTerm.toLowerCase()) ||
    skier.lastName.toLowerCase().includes(searchTerm.toLowerCase()) ||
    (skier.country?.countryName
        .toLowerCase()
        .includes(searchTerm.toLowerCase()) ??
        false);

export const SkierListView: React.FC = () => {
    const { loading, error, result: skiers } = useAsync(getSkiers, []);

    return (
        <LoadingWrapper loading={loading} error={error}>
            <ListViewWrapper
                createConfig={{
                    createText: 'Rennfahrer erstellen',
                    createUrl: '/skier/new'
                }}
            >
                <SearchContext.Consumer>
                    {search => (
                        <SkierList>
                            {skiers
                                ?.sort((s1, s2) =>
                                    s1.lastName.localeCompare(s2.lastName)
                                )
                                .filter(s => skierFilter(s, search))
                                .map(s => (
                                    <ListItemWrapper key={s.id}>
                                        <SkierListViewItem skier={s} />
                                    </ListItemWrapper>
                                ))}
                        </SkierList>
                    )}
                </SearchContext.Consumer>
            </ListViewWrapper>
        </LoadingWrapper>
    );
};
